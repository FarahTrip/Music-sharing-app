﻿// <auto-generated />
namespace Trippin_Website.Migrations
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Migrations.Infrastructure;
    using System.Resources;
    
    [GeneratedCode("EntityFramework.Migrations", "6.4.4")]
    public sealed partial class AddingGuidToPieseInsteadOfIntForID : IMigrationMetadata
    {
        private readonly ResourceManager Resources = new ResourceManager(typeof(AddingGuidToPieseInsteadOfIntForID));
        
        string IMigrationMetadata.Id
        {
            get { return "202302281200444_AddingGuidToPieseInsteadOfIntForID"; }
        }
        
        string IMigrationMetadata.Source
        {
            get { return null; }
        }
        
        string IMigrationMetadata.Target
        {
            get { return Resources.GetString("Target"); }
        }
    }
}