using System;
using System.Collections.Generic;
using DataTransferObjects;
using MySql.Data.MySqlClient;


namespace DataAccessObjects
{
    public class RelationDAO: DataAccessBase
    {
        public RelationDAO(MySqlConnection mySqlConnection)
        {
            this.mySqlConnection = mySqlConnection;
        }

        // Recupera os objetos "relation" pertencentes ao workflow especificado
        public List<RelationDTO> RetrieveRelations(int workflowId)
        {
            List<RelationDTO> relationList = new List<RelationDTO>();

            String query = "SELECT * FROM `flowManager`.`relation` WHERE owner=" + workflowId;
            MySqlCommand command = new MySqlCommand(query, this.mySqlConnection);
            MySqlDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                RelationDTO relation = new RelationDTO();
                relation.id = (int)dataReader["id"];
                relation.owner = (int)dataReader["owner"];
                relation.origin = (int)dataReader["origin"];
                relation.destinarion = (int)dataReader["destination"];

                relationList.Add(relation);
            }
            dataReader.Close();

            return relationList;
        }

        public Int64 StoreRelation(RelationDTO relation)
        {
            String commandText = "INSERT INTO `flowManager`.`relation` VALUES(null, " + relation.owner + ", " + relation.origin + ", " + relation.destinarion + ")";
            if (relation.id != 0) commandText = "UPDATE `flowManager`.`relation` SET owner = " + relation.owner + ", origin = '" + relation.origin + "', destination = " + relation.destinarion + " WHERE id = " + relation.id;
            commandText = commandText + ';' + Environment.NewLine + "SELECT LAST_INSERT_ID()";

            MySqlCommand command = new MySqlCommand(commandText, this.mySqlConnection);
            Int64 insertId = (Int64)command.ExecuteScalar();
            if (relation.id != 0) insertId = relation.id;

            return insertId;
        }
    }

}
