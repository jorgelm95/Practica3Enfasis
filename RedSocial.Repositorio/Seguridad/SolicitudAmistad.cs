using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesDominio = RedSocial.Dominio.Seguridad;
using repositorio = RedSocial.Repositorio.Seguridad;
namespace RedSocial.Repositorio.Seguridad
{
  public class SolicitudAmistad
    {

      repositorio.Usuario repoUsuario = new repositorio.Usuario();

      private RedSocialContexto contexto;

      public SolicitudAmistad()
      {
          contexto = new RedSocialContexto();
      }


      public void EnviarSolicitudAmistad(Guid IDusuarioEmisor, Guid IDusuarioReceptor)
      {
          var usuarioEmisor = repoUsuario.consultarUsuarioPorId(IDusuarioEmisor);
          var usurioReceptor = repoUsuario.consultarUsuarioPorId(IDusuarioReceptor);

          EntidadesDominio.SolicitudAmistad solictid = new EntidadesDominio.SolicitudAmistad();
          solictid.UsuarioEnviaSolicitud = usuarioEmisor;
          solictid.usuarioRecibeSolicitud = usurioReceptor;
          solictid.AceptaSolicitud = false;
          solictid.FechaAmistad = DateTime.Now;
          contexto.Solicitudes.Add(solictid);
          contexto.SaveChanges();
      }

      public void AceptarSolicitud(Guid id)
      {
          var solicitudAceptar = consultarSolicitud(id);
          solicitudAceptar.AceptaSolicitud = true;

          repositorio.Usuario repoUsuario = new repositorio.Usuario();

          var usuarioActualizarListaAmigos = repoUsuario.consultarUsuarioPorId(solicitudAceptar.UsuarioEnviaSolicitud.Id);
          usuarioActualizarListaAmigos.Amigos.Add(solicitudAceptar.usuarioRecibeSolicitud);
          contexto.SaveChanges();
          
      }

      public EntidadesDominio.SolicitudAmistad consultarSolicitud(Guid id)
      {
          var solicitudBuscada = contexto.Solicitudes.FirstOrDefault(s=> s.Id == id);
          return solicitudBuscada;
      }


      public void EliminarSolicitud(Guid id)
      {
          var solicitudEliminar = consultarSolicitud(id);
          contexto.Solicitudes.Remove(solicitudEliminar);
          contexto.SaveChanges();
      }


    }
}
