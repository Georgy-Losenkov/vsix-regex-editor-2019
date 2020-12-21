using System;
using System.Collections.Generic;
using Losenkov.RegexEditor.UI.Model;

namespace Losenkov.RegexEditor.UI.ViewModel
{
    static class RegexRunner
    {
        const Int32 SanityLimit = 1000;

        #region Checker
        sealed class Checker
        {
            readonly Int32 m_maxNodes;
            readonly DateTime m_maxTime;
            Int32 m_currNodes;
            DateTime m_currTime;

            public Checker(Int32 maxNodes, Int32 timeout)
            {
                m_maxNodes = maxNodes;
                m_maxTime = DateTime.Now.AddMilliseconds(timeout);
                m_currNodes = 0;
                m_currTime = DateTime.Now;

                State = ExecutionState.None;
            }

            public void Adjust()
            {
                m_currNodes++;
                m_currTime = DateTime.Now;
                State =
                    (m_maxNodes < m_currNodes) ? ExecutionState.Truncated :
                    (m_maxTime < m_currTime) ? ExecutionState.TimedOut :
                    ExecutionState.None;
            }

            public ExecutionState State { get; private set; }

            public Boolean Terminated
            {
                get { return State != ExecutionState.None; }
            }
        }
        #endregion
        #region ExecuteCode()
        static void ExecuteCsCode(IOptions snapshot, ResultsViewModel results, RegexMethod regexMethod)
        {
            var template = new Generators.CsCodeTemplate(snapshot, regexMethod);
            results.SetText(template.TransformText());
        }
        static void ExecuteVbCode(IOptions snapshot, ResultsViewModel results, RegexMethod regexMethod)
        {
            var template = new Generators.VbCodeTemplate(snapshot, regexMethod);
            results.SetText(template.TransformText());
        }
        #endregion
        #region ExecuteMatch()
        static void ExecuteMatch(IOptions snapshot, ResultsViewModel results)
        {
            var regex = snapshot.CreateRegex(1000);
            var groupNames = regex.GetGroupNames();
            var groupNumbers = regex.GetGroupNumbers();

            #region build tree
            if (snapshot.MultilineInput)
            {
                var checker = new Checker(SanityLimit, 5000);
                var nodes = new List<LineNode>();
                var lines = Line.Split(snapshot.InputText);

                for (var i = 0; i < lines.Length; i++)
                {
                    if (checker.Terminated)
                    {
                        break;
                    }

                    var line = lines[i];
                    var lineNode = new LineNode(line, i);
                    nodes.Add(lineNode);

                    checker.Adjust();

                    var match = regex.Match(snapshot.InputText, line.Index, line.Length);

                    if (match.Success)
                    {
                        for (var j = 0; match.Success; j++, match = match.NextMatch())
                        {
                            if (checker.Terminated)
                            {
                                break;
                            }

                            var matchNode = new MatchNode(match, j);
                            ((IList<MatchNode>)lineNode.Matches).Add(matchNode);

                            checker.Adjust();

                            for (var k = 0; k < groupNames.Length; k++)
                            {
                                if (checker.Terminated)
                                {
                                    break;
                                }

                                var number = groupNumbers[k];
                                var name = groupNames[k];
                                var group = match.Groups[number];

                                var groupNode = new GroupNode(group, k, number, name);
                                ((IList<GroupNode>)matchNode.Groups).Add(groupNode);

                                checker.Adjust();

                                for (var l = 0; l < group.Captures.Count; l++)
                                {
                                    if (checker.Terminated)
                                    {
                                        break;
                                    }

                                    var capture = group.Captures[l];
                                    var captureNode = new CaptureNode(capture, l);
                                    ((IList<CaptureNode>)groupNode.Captures).Add(captureNode);

                                    checker.Adjust();
                                }
                            }
                        }
                    }
                    else
                    {
                        var matchNode = new MatchNode(match, 0);
                        ((IList<MatchNode>)lineNode.Matches).Add(matchNode);

                        checker.Adjust();
                    }
                }

                results.SetTree(nodes, checker.State);
            }
            else
            {
                var checker = new Checker(SanityLimit, 5000);
                var nodes = new List<MatchNode>();
                var match = regex.Match(snapshot.InputText);

                if (match.Success)
                {
                    for (var j = 0; match.Success; j++, match = match.NextMatch())
                    {
                        if (checker.Terminated)
                        {
                            break;
                        }

                        var matchNode = new MatchNode(match, j);
                        nodes.Add(matchNode);

                        checker.Adjust();

                        for (var k = 0; k < groupNames.Length; k++)
                        {
                            if (checker.Terminated)
                            {
                                break;
                            }

                            var number = groupNumbers[k];
                            var name = groupNames[k];
                            var group = match.Groups[number];

                            var groupNode = new GroupNode(group, k, number, name);
                            ((IList<GroupNode>)matchNode.Groups).Add(groupNode);

                            checker.Adjust();

                            for (var l = 0; l < group.Captures.Count; l++)
                            {
                                if (checker.Terminated)
                                {
                                    break;
                                }

                                var capture = group.Captures[l];
                                var captureNode = new CaptureNode(capture, l);
                                ((IList<CaptureNode>)groupNode.Captures).Add(captureNode);

                                checker.Adjust();
                            }
                        }
                    }
                }
                else
                {
                    var matchNode = new MatchNode(match, 0);
                    nodes.Add(matchNode);

                    checker.Adjust();
                }

                results.SetTree(nodes, checker.State);
            }
            #endregion
        }
        #endregion
        #region ExecuteReplace()
        static void ExecuteReplace(IOptions snapshot, ResultsViewModel results)
        {
            var regex = snapshot.CreateRegex(1000);

            if (snapshot.MultilineInput)
            {
                var checker = new Checker(SanityLimit, 5000);
                var lines = Line.Split(snapshot.InputText);
                var list = new List<LineReplacement>();

                for (var i = 0; i < lines.Length; i++)
                {
                    if (checker.Terminated)
                    {
                        break;
                    }

                    var line = lines[i];
                    var result = regex.Replace(line.Value, snapshot.ReplacementText);
                    list.Add(new LineReplacement(line, i + 1, result));

                    checker.Adjust();
                }

                results.SetGrid(new LineReplacements(list.ToArray()), checker.State);
            }
            else
            {
                var input = snapshot.InputText;
                var result = regex.Replace(input, snapshot.ReplacementText);

                results.SetText(result);
            }
        }
        #endregion
        #region ExecuteSplit()
        static void ExecuteSplit(IOptions snapshot, ResultsViewModel results)
        {
            var regex = snapshot.CreateRegex(1000);

            if (snapshot.MultilineInput)
            {
                var checker = new Checker(SanityLimit, 5000);
                var lines = Line.Split(snapshot.InputText);
                var list = new List<LineFragment>();
                for (var i = 0; i < lines.Length; i++)
                {
                    if (checker.Terminated)
                    {
                        break;
                    }

                    var line = lines[i];
                    var result = regex.Split(line.Value);
                    checker.Adjust();

                    if (result.Length == 0)
                    {
                        list.Add(new LineFragment(line, i + 1, null, String.Empty));

                        checker.Adjust();
                    }
                    else
                    {
                        for (var j = 0; j < result.Length; j++)
                        {
                            if (checker.Terminated)
                            {
                                break;
                            }

                            list.Add(new LineFragment(line, i + 1, j, result[j]));

                            checker.Adjust();
                        }
                    }
                }

                results.SetGrid(new LineFragments(list.ToArray()), checker.State);
            }
            else
            {
                var checker = new Checker(SanityLimit, 5000);
                var list = new List<InputFragment>();
                var input = snapshot.InputText;
                var result = regex.Split(input);
                if (result.Length == 0)
                {
                    list.Add(new InputFragment(null, String.Empty));

                    checker.Adjust();
                }
                else
                {
                    for (var j = 0; j < result.Length; j++)
                    {
                        if (checker.Terminated)
                        {
                            break;
                        }

                        list.Add(new InputFragment(j, result[j]));

                        checker.Adjust();
                    }
                }

                results.SetGrid(new InputFragments(list.ToArray()), checker.State);
            }
        }
        #endregion

        public static void Execute(IOptions snapshot, ResultsViewModel results, RegexMethod regexMethod, TesterMode testerMode)
        {
            try
            {
                results.Reset();

                if (testerMode == TesterMode.Invoke)
                {
                    if (regexMethod == RegexMethod.Match)
                    {
                        ExecuteMatch(snapshot, results);
                    }
                    else if (regexMethod == RegexMethod.Replace)
                    {
                        ExecuteReplace(snapshot, results);
                    }
                    else if (regexMethod == RegexMethod.Split)
                    {
                        ExecuteSplit(snapshot, results);
                    }
                }
                else if (testerMode == TesterMode.CsCode)
                {
                    ExecuteCsCode(snapshot, results, regexMethod);
                }
                else if (testerMode == TesterMode.VbCode)
                {
                    ExecuteVbCode(snapshot, results, regexMethod);
                }
            }
            catch (Exception ex)
            {
                results.SetText(ex.ToString());
            }
        }
    }
}