using System.Text.RegularExpressions;
using Microsoft.SqlServer.Dac.Deployment;
using Microsoft.SqlServer.Dac.Model;

namespace AgileSqlClub.SqlPackageFilter.Filter
{
  internal class DataLossCheckStep
  {
    public bool IsDataLossCheck => ObjectName != null;
    public ObjectIdentifier ObjectName { get; private set; }

    public DataLossCheckStep(DeploymentStep step)
    {
      var scriptStep = step as DeploymentScriptStep;

      var scriptLines = scriptStep?.GenerateTSQL();
      var script = scriptLines != null && scriptLines.Count > 0 ? scriptLines[0] : "";

      ObjectName = TableDropIdentifier(script) ?? ColumnDropIdentifier(script);
    }

    public ObjectIdentifier TableDropIdentifier(string script)
    {
      var match = Regex.Match(script, @"Table (.*) is being dropped\.  Deployment will halt if the table contains data\.");

      if (!match.Success)
      {
        return null;
      }

      return ObjectIdentifierFactory.Create(match.Groups[1].Value);
    }

    public ObjectIdentifier ColumnDropIdentifier(string script)
    {
      var match = Regex.Match(script, @"The column (\[\w*\]\.\[\w*\])\.\[\w*\] is being dropped, data loss could occur");

      if (!match.Success)
      {
        return null;
      }

      return ObjectIdentifierFactory.Create(match.Groups[1].Value);
    }
  }
}