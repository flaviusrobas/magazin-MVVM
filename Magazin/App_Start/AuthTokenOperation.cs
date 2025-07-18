using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Description;

namespace Magazin.App_Start
{
    public class AuthTokenOperation : IDocumentFilter
    {
        public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
        {
            swaggerDoc.paths.Add("/token", new PathItem
            {
                post = new Operation
                {
                    tags = new List<string> { "Auth" },
                    consumes = new List<string> { "application/x-www-form-urlencoded" },
                },
                parameters = new List<Parameter>
                {
                     new Parameter
                     {
                          name = "grant_type",
                          @in = "formData",
                          type = "string",
                          required = true,
                          description = "The type of grant being requested, e.g., 'password'.",
                          @default = "password"

                     },
                     new Parameter
                     {
                          name = "username",
                          @in = "formData",
                          type = "string",
                          required = false,
                          description = "The username of the user."
                     },
                     new Parameter
                     {
                          name = "password",
                          @in = "formData",
                          type = "string",
                          required = false,
                          description = "The password of the user."
                     }
                },
            });
        }
    }
}