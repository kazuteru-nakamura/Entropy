// Copyright (c) Microsoft Open Technologies, Inc.
// All Rights Reserved
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// THIS CODE IS PROVIDED *AS IS* BASIS, WITHOUT WARRANTIES OR
// CONDITIONS OF ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING
// WITHOUT LIMITATION ANY IMPLIED WARRANTIES OR CONDITIONS OF
// TITLE, FITNESS FOR A PARTICULAR PURPOSE, MERCHANTABLITY OR
// NON-INFRINGEMENT.
// See the Apache 2 License for the specific language governing
// permissions and limitations under the License.

using System.Threading.Tasks;
using Config.CustomSource.Web;
using Microsoft.AspNet.Abstractions;
using Microsoft.AspNet.ConfigurationModel;

public class Startup
{
    public void Configuration(IBuilder app)
    {
        var config = new Configuration
        {
            new MyConfigSource()
        };

        app.Run(async ctx =>
        {
            ctx.Response.ContentType = "text/plain";
            await DumpConfig(ctx.Response, config);
        });
    }

    private static async Task DumpConfig(HttpResponse response, IConfiguration config, string indentation = "")
    {
        foreach (var child in config.GetSubKeys())
        {
            await response.WriteAsync(indentation + "[" + child.Key + "] " + config.Get(child.Key) + "\r\n");
            await DumpConfig(response, child.Value, indentation + "  ");
        }
    }
}