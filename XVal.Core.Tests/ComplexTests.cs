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
        public int Id { get; set; }
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
                    new Project
                    {
                        Id = 1,
                        Manager = new Resource
                        {
                            Id = 1,
                            PrimarySkill = new Skill{ Id = 3 },
                            SecondarySkills = new List<Skill>{ new Skill{ Id = 4 }, new Skill{ Id = 5 }}
                        },
                        Resources = new List<Resource>
                        {
                            new Resource
                            {
                                Id = 2,
                                PrimarySkill = new Skill{ Id = 4 },
                                SecondarySkills = new List<Skill>{ new Skill{ Id = 3 }, new Skill{ Id = 5 }}
                            },
                            new Resource
                            {
                                Id = 3,
                                PrimarySkill = new Skill{ Id = 5 },
                                SecondarySkills = new List<Skill>{ new Skill{ Id = 3 }, new Skill{ Id = 4 }}
                            },
                        }
                    },
                    ValidationResult.Failed(
                    "Validation errors for project. Project Id = 1" + Environment.NewLine +
                    "Project name is mandatory. Project Id = 1" + Environment.NewLine +
                    "Validation errors for manager" + Environment.NewLine +
                    "Validation errors for the resource" + Environment.NewLine +
                    "Resource name is mandatory. Resource Id = 1" + Environment.NewLine +
                    "Resource Id = 1" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 3" + Environment.NewLine +
                    "Resource Id = 1" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 4" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 5" + Environment.NewLine +
                    "Validation errors for resources" + Environment.NewLine +
                    "Validation errors for the resource" + Environment.NewLine +
                    "Resource name is mandatory. Resource Id = 2" + Environment.NewLine +
                    "Resource Id = 2" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 4" + Environment.NewLine +
                    "Resource Id = 2" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 3" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 5" + Environment.NewLine +
                    "Validation errors for the resource" + Environment.NewLine +
                    "Resource name is mandatory. Resource Id = 3" + Environment.NewLine +
                    "Resource Id = 3" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 5" + Environment.NewLine +
                    "Resource Id = 3" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 3" + Environment.NewLine +
                    "Skill name is mandatory. Skill Id = 4")
                },
            };

        private static CompositeValidationRule<Project> BuildProjectValidationRule()
        {
            var entityNameRule = ValidationRule.For<Entity>()
                .Validate(e => !string.IsNullOrWhiteSpace(e.Name))
                .Message(e => $"{e.GetType().Name} name is mandatory. {e.GetType().Name} Id = {e.Id}")
                .Build();

            var resourceSecondarySkillsRule = ValidationRule.For<Resource>()
                .ForChildren(m => m.SecondarySkills)
                .Validate(entityNameRule)
                .Message(m => $"Resource Id = {m.Id}")
                .Build();

            var resourcePrimarySkillRule = ValidationRule.For<Resource>()
                .ForChild(m => m.PrimarySkill)
                .Validate(entityNameRule)
                .Message(m => $"Resource Id = {m.Id}")
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
                .Message(p => $"Validation errors for project. Project Id = {p.Id}")
                .Build();
            return projectRule;
        }
    }
}
