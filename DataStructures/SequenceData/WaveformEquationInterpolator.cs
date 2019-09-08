using System;
using System.Collections.Generic;
using System.Text;
using dotMath;
using System.Diagnostics;

namespace DataStructures
{
    public class WaveformEquationInterpolator
    {
        private static int stackCount = 0;

        public static string getEquationStatusString(string equationString, List<Variable> existingVariables, List<Waveform> existingCommonWaveforms)
        {
            dotMath.EqCompiler eq = new EqCompiler(equationString, true);
            try
            {
                eq.Compile();
            }
            catch (Exception e)
            {
                return e.Message;
            }

            string[] foundvars = eq.GetVariableList();
            if (foundvars != null)
            {

                List<string> foundVariables = new List<string>(eq.GetVariableList());

                List<string> existingVariableNames = new List<string>();
                List<string> existingCommonWaveformNames = new List<string>();
                foreach (Variable var in existingVariables)
                {
                    if (!existingVariableNames.Contains(var.VariableName))
                    {
                        existingVariableNames.Add(var.VariableName);
                    }
                }

                if (existingCommonWaveforms != null)
                {
                    foreach (Waveform wf in existingCommonWaveforms)
                    {
                        if (!existingCommonWaveformNames.Contains(wf.WaveformName))
                        {
                            existingCommonWaveformNames.Add(wf.WaveformName);
                        }
                    }
                }

                foreach (string foundVariableName in foundVariables)
                {
                    if (foundVariableName != "t")
                    {
                        if (!(existingVariableNames.Contains(foundVariableName)))
                        {
                            if (!existingCommonWaveformNames.Contains(foundVariableName))
                            {
                                return "No variable or common waveform found with name " + foundVariableName;
                            }
                        }
                    }
                }
            }

            return "Valid equation.";

        }

        public static void getEquationInterpolation(string equationString, double startTime, double endTime, int nSamples, int startIndex, double[] output, List<Variable> existingVariables, List<Waveform> existingCommonWaveforms, double wfDuration)
        {
            string status = getEquationStatusString(equationString, existingVariables, existingCommonWaveforms);

            if (status == "Valid equation.")
            {
                // HERE WE GO!

                dotMath.EqCompiler eq = new EqCompiler(equationString, true);
                // This should not fail, because if it did we would have caught it with the status check
                eq.Compile();

                string[] foundvars = eq.GetVariableList();
                Dictionary<string, Waveform> waveformDependentVariables = new Dictionary<string, Waveform>();

                bool usingTimeVariable = false;
                bool usingCommonWaveforms = false;

                if (foundvars != null)
                {
                    foreach (string varname in foundvars)
                    {
                        bool variableMapped = false;
                        if (varname != "t")
                        {
                            foreach (Variable var in existingVariables)
                            {
                                if (var.VariableName == varname)
                                {
                                    eq.SetVariable(varname, var.VariableValue);
                                    variableMapped = true;
                                }
                            }
                        }

                        if (varname == "t")
                        {
                            usingTimeVariable = true;
                            variableMapped = true;
                        }



                        // variable named varname has not yet been mapped. Thus it must be a common waveform
                        if (!variableMapped)
                        {
                            if (existingCommonWaveforms != null)
                            {
                                foreach (Waveform wf in existingCommonWaveforms)
                                {
                                    if (wf.WaveformName == varname)
                                    {
                                        if (!waveformDependentVariables.ContainsKey(varname))
                                        {
                                            waveformDependentVariables.Add(varname, wf);
                                            usingCommonWaveforms = true;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }

                Dictionary<Waveform, double[]> commonWaveformInterpolations = new Dictionary<Waveform, double[]>();
                if (usingCommonWaveforms)
                {
                    foreach (Waveform wf in waveformDependentVariables.Values)
                    {
                        WaveformEquationInterpolator.stackCount++;
                        if (stackCount > 40)
                        {
                            stackCount = 0;
                            throw new InvalidDataException("Stack count has reached 40 when attempting to interpolation an equation waveform. You have probably created a recursive reference loop using waveform equations. Please remove the offending circular references. Aborting interpolation.");
                        }
                        double[] interpol = wf.getInterpolation(nSamples, startTime, endTime, existingVariables, existingCommonWaveforms);
                        commonWaveformInterpolations.Add(wf, interpol);
                        stackCount--;
                    }
                }

                for (int i = 0; i < nSamples; i++)
                {
                    if (usingTimeVariable)
                    {
                        double time = i * (endTime - startTime) / (double)nSamples + startTime;

                        if (time > wfDuration)
                            time = wfDuration;

                        eq.SetVariable("t", time);
                    }

                    if (usingCommonWaveforms)
                    {
                        foreach (string vname in waveformDependentVariables.Keys)
                        {
                            eq.SetVariable(vname, commonWaveformInterpolations[waveformDependentVariables[vname]][i]);
                        }
                    }

                    try
                    {
                        double val = eq.Calculate();
                        if ((!double.IsInfinity(val)) && (!double.IsNaN(val)))
                        {
                            output[i + startIndex] = eq.Calculate();
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
            }
        }

        public static string CutEquationIntoParts(string equationString, double startTime, List<Variable> existingVariables, List<Waveform> existingCommonWaveforms, HashSet<String> commonWaveforms, bool eqnIncludesCommonWaveforms)
        {
            string status = getEquationStatusString(equationString, existingVariables, existingCommonWaveforms);

            if (status == "Valid equation.")
            {   
                dotMath.EqCompiler eq = new EqCompiler(equationString, true);
                eq.Compile();

                string[] varList = eq.GetVariableList();
                bool foundTimeVariable = false;
                foreach (String var in varList)
                {
                    //If the equation depends on a commonWaveform then we can't deal with it simply
                    if (commonWaveforms.Contains(var))
                    {
                        eqnIncludesCommonWaveforms = true;
                        return "";
                    }
                    else if (var == "t")
                    { foundTimeVariable = true; }
                }
                if (foundTimeVariable) //we need to edit the output string :(
                {
                    //Test each character in the equation string to see if it is the variable t
                    HashSet<char> acceptableChars = new HashSet<char>("+-*/^ ()=<>&|".ToCharArray());
                    //This hash set contains characters that are allowed to be on either side of t. 
                    //If a character t has something other than one of these beside it then it can't be the
                    //variable t that we are looking for. Also notice that we're running through the string
                    //backwards so that the indexing doesn't get messed up when we insert characters.
                    for (int ind = equationString.Length-1; ind >= 0; ind--)
                    {
                        if (equationString[ind].ToString() == "t" && (ind == 0 || acceptableChars.Contains(equationString[ind - 1]))
                            && (ind == equationString.Length - 1 || acceptableChars.Contains(equationString[ind + 1])))
                        {
                            //Having found an instance of the variable t, replace it in the string with (t+startTime)
                            equationString = equationString.Insert(ind + 1, "+" + startTime + ")");
                            equationString = equationString.Insert(ind,"(");
                            ind = ind--;
                        }
                    }
                }
            }
            return equationString;
        }
    }
}
