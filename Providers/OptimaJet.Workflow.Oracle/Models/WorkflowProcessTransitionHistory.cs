﻿using System;
using System.Data;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

// ReSharper disable once CheckNamespace
namespace OptimaJet.Workflow.Oracle
{
    public class WorkflowProcessTransitionHistory : DbObject<WorkflowProcessTransitionHistory>
    {
        public string ActorIdentityId { get; set; }
        public string ExecutorIdentityId { get; set; }
        public string FromActivityName { get; set; }
        public string FromStateName { get; set; }
        public Guid Id { get; set; }
        public bool IsFinalised { get; set; }
        public Guid ProcessId { get; set; }
        public string ToActivityName { get; set; }
        public string ToStateName { get; set; }
        public string TransitionClassifier { get; set; }
        public DateTime TransitionTime { get; set; }
        public string TriggerName { get; set; }

        static WorkflowProcessTransitionHistory()
        {
            DbTableName = "WorkflowProcessTransitionH";
        }

        public WorkflowProcessTransitionHistory()
        {
            DBColumns.AddRange(new[]{
                new ColumnInfo {Name="Id", IsKey = true, Type = OracleDbType.Raw},
                new ColumnInfo {Name="ActorIdentityId"},
                new ColumnInfo {Name="ExecutorIdentityId"},
                new ColumnInfo {Name="FromActivityName"},
                new ColumnInfo {Name="FromStateName"},
                new ColumnInfo {Name="IsFinalised", Type = OracleDbType.Byte},
                new ColumnInfo {Name="ProcessId", Type = OracleDbType.Raw},
                new ColumnInfo {Name="ToActivityName"},
                new ColumnInfo {Name="ToStateName"},
                new ColumnInfo {Name="TransitionClassifier"},
                new ColumnInfo {Name="TransitionTime", Type = OracleDbType.Date },
                new ColumnInfo {Name="TriggerName"}
            });
        }

        public override object GetValue(string key)
        {
            switch (key)
            {
                case "Id":
                    return Id.ToByteArray();
                case "ActorIdentityId":
                    return ActorIdentityId;
                case "ExecutorIdentityId":
                    return ExecutorIdentityId;
                case "FromActivityName":
                    return FromActivityName;
                case "FromStateName":
                    return FromStateName;
                case "IsFinalised":
                    return IsFinalised ? "1": "0";
                case "ProcessId":
                    return ProcessId.ToByteArray();
                case "ToActivityName":
                    return ToActivityName;
                case "ToStateName":
                    return ToStateName;
                case "TransitionClassifier":
                    return TransitionClassifier;
                case "TransitionTime":
                    return TransitionTime;
                case "TriggerName":
                    return TriggerName;
                default:
                    throw new Exception(string.Format("Column {0} is not exists", key));
            }
        }

        public override void SetValue(string key, object value)
        {
            switch (key)
            {
                case "Id":
                    Id = new Guid((byte[])value);
                    break;
                case "ActorIdentityId":
                    ActorIdentityId = value as string;
                    break;
                case "ExecutorIdentityId":
                    ExecutorIdentityId = value as string;
                    break;
                case "FromActivityName":
                    FromActivityName = value as string;
                    break;
                case "FromStateName":
                    FromStateName = value as string;
                    break;
                case "IsFinalised":
                    IsFinalised = (string)value == "1";
                    break;
                case "ProcessId":
                    ProcessId = new Guid((byte[])value);
                    break;
                case "ToActivityName":
                    ToActivityName = value as string;
                    break;
                case "ToStateName":
                    ToStateName = value as string;
                    break;
                case "TransitionClassifier":
                    TransitionClassifier = value as string;
                    break;
                case "TransitionTime":
                    TransitionTime = (DateTime)value;
                    break;
                case "TriggerName":
                    TriggerName = value as string;
                    break;
                default:
                    throw new Exception(string.Format("Column {0} is not exists", key));
            }
        }

        public static async Task<int> DeleteByProcessIdAsync(OracleConnection connection, Guid processId)
        {
            return await ExecuteCommandAsync(connection,
                $"DELETE FROM {ObjectName} WHERE PROCESSID = :processid",
                new OracleParameter("processid", OracleDbType.Raw, processId.ToByteArray(), ParameterDirection.Input)).ConfigureAwait(false);
        }

        public static async Task<WorkflowProcessTransitionHistory[]> SelectByProcessIdAsync(OracleConnection connection, Guid processId)
        {
            string selectText = $"SELECT * FROM {ObjectName} WHERE PROCESSID = :processid";

            var p1 = new OracleParameter("processid", OracleDbType.Raw, processId.ToByteArray(), ParameterDirection.Input);

            return await SelectAsync(connection, selectText, p1).ConfigureAwait(false);
        }
    }
}
