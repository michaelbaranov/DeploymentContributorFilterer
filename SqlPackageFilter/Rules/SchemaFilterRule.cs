using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using AgileSqlClub.SqlPackageFilter.DacExtensions;
using AgileSqlClub.SqlPackageFilter.Filter;
using Microsoft.SqlServer.Dac.Deployment;
using Microsoft.SqlServer.Dac.Model;

namespace AgileSqlClub.SqlPackageFilter.Rules
{
    public class SchemaFilterRule : FilterRule
    {
        public SchemaFilterRule(FilterOperation operation, string match, MatchType matchType) : base(operation, match, matchType)
        {
        }

        public override bool Matches(ObjectIdentifier name, ModelTypeClass type, DeploymentStep step = null)
        {
            var schemaName = name.GetSchemaName(type);
            if (string.IsNullOrEmpty(schemaName) && step != null)
            {
                // Trying to get schema name from sql statement
                schemaName = GetSchemaName(step);
            }
            return Matches(schemaName);
        }

        // Get schema name from alter statement
        private string GetSchemaName(DeploymentStep step)
        {
            var sql = step.GenerateTSQL();

            var alterStatement = sql.Where(s => s.StartsWith("ALTER TABLE")).FirstOrDefault();

            if (alterStatement == null)
            {
                return string.Empty;
            }

            var regex = new Regex("\\[\\w*\\]");
            return regex.Match(alterStatement).Value;
        }
    }
}