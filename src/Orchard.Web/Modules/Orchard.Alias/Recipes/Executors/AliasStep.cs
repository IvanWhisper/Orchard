﻿using System;
using System.Web;
using System.Web.Routing;
using System.Linq;
using Orchard.Alias;
using Orchard.Logging;
using Orchard.Recipes.Models;
using Orchard.Recipes.Services;


namespace Orchard.Alias.Recipes.Executors {
    public class AliasStep : RecipeExecutionStep {
        private readonly IAliasService _aliasService;

        public AliasStep(IAliasService aliasService)        
        {
            _aliasService = aliasService;
        }

        public override string Name {
            get { return "Aliases"; }
        }

        public override void Execute(RecipeExecutionContext context) {           

            foreach (var aliasElement in context.RecipeStep.Step.Elements()) {
                var aliasPath = aliasElement.Attribute("Path").Value;

                Logger.Information("Processing alias '{0}'.", aliasPath);

                //var path = aliasElement.Attribute("Path").Value;
                var rvd = new RouteValueDictionary();

                var routeValuesElement = aliasElement.Descendants("RouteValues").FirstOrDefault();

                if (routeValuesElement != null)
                {
                    foreach (var routeValue in routeValuesElement.Descendants("Add"))
                    {
                        rvd.Add(routeValue.Attribute("Key").Value, routeValue.Attribute("Value").Value);
                    }
                }
                                
                try {
                        _aliasService.Set(aliasPath, rvd, "Custom", false);                
                    }
                
                catch (Exception ex) {
                    Logger.Error(ex, "Error while processing alias '{0}'.", aliasPath);
                    throw;
                }
    
            }
        }

        /*
  <Aliases>
    <Alias Path="Profile/Edit" Area="Custom.Profile">
      <RouteValues>
        <Add Key="area" Value="Custom.Profile" />
        <Add Key="controller" Value="Profile" />
        <Add Key="action" Value="Edit" />
      </RouteValues>
    </Alias>
 */
        //Enable any features that are in the list, disable features that aren't in the list
        //public override void Execute(RecipeExecutionContext context)
        //{
        //    if (!String.Equals(context.RecipeStep.Name, "Aliases", StringComparison.OrdinalIgnoreCase))
        //    {
        //        return;
        //    }

        //    var aliasElements = context.RecipeStep.Step.Descendants("Alias");

        //    foreach (var aliasElement in aliasElements)
        //    {
        //        var path = aliasElement.Attribute("Path").Value;
        //        var rvd = new RouteValueDictionary();

        //        var routeValuesElement = aliasElement.Descendants("RouteValues").FirstOrDefault();

        //        if (routeValuesElement != null)
        //        {
        //            foreach (var routeValue in routeValuesElement.Descendants("Add"))
        //            {
        //                rvd.Add(routeValue.Attribute("Key").Value, routeValue.Attribute("Value").Value);
        //            }
        //        }

        //        _aliasService.Set(path, rvd, "Custom");
        //    }

        //    //remove all local pathys that are not present in the remote export
        //    var allRemotePaths = recipeContext.RecipeStep.Step.XPathSelectElements("Paths/Add").Select(e => e.Attribute("Path").Value);
        //    var allLocalPaths = _aliasService.List().Select(t => t.Item1).ToList();

        //    foreach (var path in allLocalPaths.Where(p => !allRemotePaths.Contains(p)))
        //    {
        //        _aliasService.Delete(path);
        //    }


        //    recipeContext.Executed = true;
        //}
    }
}