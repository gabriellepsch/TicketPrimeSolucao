public static class EventosController{
    private static List<Evento> Eventos = new();
    private static int idAtual = 1;
    public static void ListarEventos(this WebApplication app){
        app.MapGet("/api/eventos/listar", () => 
        {
            return Results.Ok(Eventos);
        });
    }
    public static void CadastrarEventos(this WebApplication app){
        app.MapPost("/api/eventos/cadastrar", (Evento novoEvento) => 
        {

            if(Eventos.Any(e => e.Nome == novoEvento.Nome)){
                return Results.BadRequest("O nome de evento informado já está cadastrado");
            }

            novoEvento.Id = idAtual;
            idAtual++;

            Eventos.Add(novoEvento);
            return Results.Ok(novoEvento);
        });
    }
    
}

public class Evento{
    public int Id {get;set;}
    public string Nome {get;set;} = "";
    public string Local {get;set;} = "";
    public int QuantidadeIngressos {get;set;}
    public float ValorIngresso {get;set;}
}