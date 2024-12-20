using System.Reflection.Metadata;
using NSwag;

namespace BoardGameAPI.Configuration;

public static class SwaggerExamples
{
   public static void AddExamples(OpenApiDocument document) 
   {
       AddBoardGameExample(document);
       AddBoardGameUpdateExample(document);
       AddUserExample(document);
       AddUserBoardGameExample(document);
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

   private static void AddUserExample(OpenApiDocument document) 
   {
       var schema = document.Components.Schemas["CreateUserRequest"]; 
       if (schema != null)
       {
           schema.Example = new
           {
               Email = "bsadick@example.com",
               FirstName = "Bob",
               LastName = "Smith",
               Handle = "h3x" 
           };
       }
   }

   private static void AddUserBoardGameExample(OpenApiDocument document)
   {
       var schema = document.Components.Schemas["CreateUserBoardGameRequest"]; 
       var game = new BoardGame()
       {
            Name = "Splendor",
            Slug = "splendor",
            Description = "A Game of Gems and Deciet"
       };
       if (schema != null)
       {
           schema.Example = new
           {
               Email = "bsadick@example.com",
               BoardGame = game
           };
       }
   }
}