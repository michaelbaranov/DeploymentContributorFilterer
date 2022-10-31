using System.Text.RegularExpressions;
using AgileSqlClub.SqlPackageFilter.Filter;
using Microsoft.SqlServer.Dac.Deployment;
using Microsoft.SqlServer.Dac.Model;

namespace AgileSqlClub.SqlPackageFilter.Rules
{
  public class FilterRule
  {
    public readonly string Match;
    public readonly MatchType MatchType;
    public readonly Regex Regex;
    public readonly FilterOperation RuleFilterOperation;

    public FilterRule()
    {

    }

    public FilterRule(FilterOperation operation, string match, MatchType matchType)
    {
      RuleFilterOperation = operation;
      Match = match;
      MatchType = matchType;
      Regex = new Regex(Match, RegexOptions.Compiled);
    }

    public virtual bool Matches(ObjectIdentifier name, ModelTypeClass objectType, DeploymentStep step = null)
    {
      return false;
    }

    protected bool Matches(string text)
    {
      if (string.IsNullOrEmpty(text))
        return false;

      var matches = Regex.IsMatch(text);

      if (matches && MatchType == MatchType.DoesMatch)
        return true;

      return !matches && MatchType == MatchType.DoesNotMatch;
    }

    public virtual FilterOperation Operation() { return RuleFilterOperation; }
  }
}