using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using DataTransferObjects;


namespace DataAccessObjects
{
    public class UsuarioDAO: DataAccessBase
    {
        private Dictionary<String, String> userGroups;

        public UsuarioDAO(FbConnection firebirdConnection)
        {
            this.firebirdConnection = firebirdConnection;
            userGroups = GetUserGroups();
        }

        public List<UsuarioDTO> GetAll()
        {
            List<UsuarioDTO> userList = new List<UsuarioDTO>();

            FbCommand command = new FbCommand("SELECT * FROM IUSUARIOS", firebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                UsuarioDTO  user = new UsuarioDTO();
                user.nomeUsuario = GetStringValue(dataReader, "NMUSUARIO");
                user.nomeCompleto = GetStringValue(dataReader, "NMCOMPLETO");
                user.senha = GetStringValue(dataReader, "SENHA");
                user.email = GetStringValue(dataReader, "EMAIL");
                user.grupo = userGroups[GetStringValue(dataReader, "CDGRUPOUSU")];

                userList.Add(user);
            }
            dataReader.Close();

            return userList;
        }

        public Dictionary<String, String> GetUserGroups()
        {
            Dictionary<String, String> userGroups = new Dictionary<String, String>();

            FbCommand command = new FbCommand("SELECT * FROM IUSUARIOSGRP", firebirdConnection);
            FbDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                String codigoGrupo = GetStringValue(dataReader, "CDGRUPOUSU");
                String nomeGrupo = GetStringValue(dataReader, "NMGRUPOUSU");
                userGroups.Add(codigoGrupo, nomeGrupo);
            }
            dataReader.Close();

            return userGroups;
        }
    }

}
