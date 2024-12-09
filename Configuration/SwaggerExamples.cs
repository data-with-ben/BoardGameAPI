using NSwag;

namespace BoardGameAPI.Configuration;

public static class SwaggerExamples
{
   public static void AddExamples(OpenApiDocument document) 
   {
       AddBoardGameExample(document);
       AddBoardGameUpdateExample(document);
   }

   private static void AddBoardGameExample(OpenApiDocument document)
   {
       var schema = document.Components.Schemas["CreateBoardGameRequest"];
       if (schema != null)
       {
           schema.Example = new
           {
               name = "Splendor",
               description = "Gems and strategy game"
           };
       }
   }

   private static void AddBoardGameUpdateExample(OpenApiDocument document) 
   {
       var schema = document.Components.Schemas["UpdateBoardGameRequest"]; 
       if (schema != null)
       {
           schema.Example = new
           {
               name = "Catan",
               description = "Resource management and trading game" 
           };
       }
   }
}