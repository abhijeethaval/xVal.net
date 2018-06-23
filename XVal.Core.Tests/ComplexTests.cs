using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;
using Castle.Components.DictionaryAdapter;
using Xunit;

namespace XVal.Core.Tests
{
    public class Entity
    {
        public string Name { get; set; }
    }

    public class Project : Entity
    {
        public List<Resource> Resources { get; set; } = new List<Resource>();
        public Resource Manager { get; set; }
    }

    public class Resource : Entity
    {
        public Skill PrimarySkill { get; set; }
        public List<Skill> SecondarySkills { get; set; } = new List<Skill>();
    }

    public class Skill : Entity
    {
    }

    public class ComplexTests
    {
        [Theory]
        [MemberData(nameof(GetProjectData))]
        public void ComplexRulesWork(Project project, ValidationResult expected)
        {
            var projectRule = BuildProjectValidationRule();
            var actual = projectRule.Execute(project);
            Assert.Equal(expected, actual);
        }

        public static IEnumerable<object[]> GetProjectData() =>
            new[]
            {
                new object[]
                {
                    new Project(),
                    ValidationResult.Failed(
                        "Validation errors for project" + Environment.NewLine +
                        "Name is mandatory")
                },
                new object[]
                {
                    new Project { Manager = new Resource()},
                    ValidationResult.Failed(
                        "Validation errors for project" + Environment.NewLine +
                        "Name is mandatory" + Environment.NewLine +
                        "Validation errors for manager" + Environment.NewLine +
                        "Validation errors for the resource" + Environment.NewLine +
                        "Name is mandatory")
                },
            };

        private static CompositeValidationRule<Project> BuildProjectValidationRule()
        {
            var entityNameRule = ValidationRule.For<Entity>()
                .Validate(e => !string.IsNullOrWhiteSpace(e.Name))
                .Message("Name is mandatory")
                .Build();

            var resourceSecondarySkillsRule = ValidationRule.For<Resource>()
                .ForChildren(m => m.SecondarySkills)
                .Validate(entityNameRule)
                .Message(m => $"Resource name = {m.Name}")
                .Build();

            var resourcePrimarySkillRule = ValidationRule.For<Resource>()
                .ForChild(m => m.PrimarySkill)
                .Validate(entityNameRule)
                .Message(m => $"Resource name = {m.Name}")
                .Build();

            var resourceRule = ValidationRule.For<Resource>()
                .Validate(entityNameRule, resourcePrimarySkillRule, resourceSecondarySkillsRule)
                .Message("Validation errors for the resource")
                .Build();

            var projectManagerRule = ValidationRule.For<Project>()
                .ForChild(p => p.Manager)
                .Validate(resourceRule)
                .Message("Validation errors for manager")
                .Build();

            var projectResourcesRule = ValidationRule.For<Project>()
                .ForChildren(p => p.Resources)
                .Validate(resourceRule)
                .Message("Validation errors for resources")
                .Build();

            var projectRule = ValidationRule.For<Project>()
                .Validate(entityNameRule, projectManagerRule, projectResourcesRule)
                .Message("Validation errors for project")
                .Build();
            return projectRule;
        }
    }
}
