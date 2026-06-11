using billet_2.Models;

namespace billet_2.Services;

public class AuthService
{
    public Passageiro? UsuarioLogado { get; private set; }  // Antigo: Usuario?
    public bool EstaLogado = false;

    public void Logar(Passageiro passageiro)  // Antigo: Logar(Usuario usuario)
    {
        UsuarioLogado = passageiro;
        EstaLogado = true;
    }

    public void Deslogar()
    {
        UsuarioLogado = null;
        EstaLogado = false;
    }
}
