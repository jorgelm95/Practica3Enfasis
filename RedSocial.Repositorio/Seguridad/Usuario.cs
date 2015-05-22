using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EntidadesDominio = RedSocial.Dominio.Seguridad;

namespace RedSocial.Repositorio.Seguridad
{
    public class Usuario
    {
        RedSocialContexto usuarioContexto;
        public Usuario()
        {
            usuarioContexto = new RedSocialContexto();
        }

        public void GuardarUsuario(EntidadesDominio.Usuario usuario)
        {
            usuarioContexto.Usuarios.Add(usuario);
            usuarioContexto.SaveChanges();
        }


        public EntidadesDominio.Usuario consultarUsuarioPorId(Guid id)
        {
            var usuarioConsultar = usuarioContexto.Usuarios.FirstOrDefault(u => u.Id == id);
            return usuarioConsultar;
        }

        public void EditarUsuario(EntidadesDominio.Usuario usuario)
        {
            var usuarioEditar = consultarUsuarioPorId(usuario.Id);
            usuario.Nombre = usuarioEditar.Nombre;
            usuario.Email = usuarioEditar.Email;
            usuario.Foto = usuarioEditar.Foto;
            usuario.NombreUsuario = usuarioEditar.NombreUsuario;
            usuario.Contraseña = usuarioEditar.Contraseña;
            usuario.ConfirmacionContraseña = usuarioEditar.ConfirmacionContraseña;

            usuarioContexto.SaveChanges();
        }

        public void EliminarUsuario(EntidadesDominio.Usuario usuario)
        {
            var usuarioEliminar = consultarUsuarioPorId(usuario.Id);
            usuarioContexto.Usuarios.Remove(usuarioEliminar);
            usuarioContexto.SaveChanges();
        }


        public EntidadesDominio.Usuario Login(string nombreusuario, string contraseña)
        {
            var contador = 0;
            var usuarioValidado = usuarioContexto.Usuarios.FirstOrDefault(u => u.NombreUsuario == nombreusuario && u.Contraseña == contraseña);

            if (usuarioValidado == null)
            {
                contador = contador + 1;
                if(contador == 3)
                {
                    bloquearCuenta(usuarioValidado);

                }
                
                return usuarioValidado;
            }
            else
            {

                return usuarioValidado;
            }
        }

        public EntidadesDominio.Usuario ValidarCorreo(string correo)
        {
            var usuario = usuarioContexto.Usuarios.FirstOrDefault(u => u.Email == correo);
            return usuario;
        }


        public List<EntidadesDominio.Usuario> ListaUsuariosPorNombre(string nombre)
        {
            List<EntidadesDominio.Usuario> listaUsuarios = usuarioContexto.Usuarios.Where(u => u.Nombre.Contains(nombre)).ToList();
            return listaUsuarios;
        }

        public List<EntidadesDominio.Usuario> Amigos(Guid idusuario)
        {
            List<EntidadesDominio.Usuario> listaAmigos = new List<EntidadesDominio.Usuario>();

            var usuario = consultarUsuarioPorId(idusuario);

            listaAmigos = usuario.Amigos.ToList();

            return listaAmigos;
        }

        public void resetearUsuario(EntidadesDominio.Usuario usuario)
        {
            var usuarioResetear = consultarUsuarioPorId(usuario.Id);
            usuarioResetear.intentosFallidos = 0;
            usuarioContexto.SaveChanges();
        }

        public void bloquearCuenta(EntidadesDominio.Usuario usuarioBloquear)
        {
            var usuario = consultarUsuarioPorId(usuarioBloquear.Id);
            var numIntentos = usuario.intentosFallidos;
            if (numIntentos == 3)
            {
                usuario.BloqueoCuenta = true;
            }
            usuarioContexto.SaveChanges();

        }

    }
}
