using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using OSDevGrp.OSIntranet.Infrastructure.Interfaces.Guards;
using Rhino.Mocks;

namespace OSDevGrp.OSIntranet.Tests.Unittests.Repositories.DataProxies
{
    /// <summary>
    /// Builder which can build a database command tester.
    /// </summary>
    internal class DbCommandTestBuilder : IDbCommandTestBuilder
    {
        #region Private variables

        private readonly IDbCommand _expectedDbCommand;
        private readonly IList<IDbDataParameter> _dataParameterCollection = new List<IDbDataParameter>();

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a builder which can build a database command tester.
        /// </summary>
        /// <param name="commandText">The expected command text.</param>
        /// <param name="commandType">The expected command type.</param>
        /// <param name="commandTimeout">The expected command timeout.</param>
        public DbCommandTestBuilder(string commandText, CommandType commandType = CommandType.Text, int commandTimeout = 30)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(commandText, nameof(commandText));

            IDataParameterCollection dataParameterCollection = MockRepository.GenerateMock<IDataParameterCollection>();
            dataParameterCollection.Stub(m => m.Count)
                .WhenCalled(e => e.ReturnValue = _dataParameterCollection.Count)
                .Return(0)
                .Repeat.Any();
            dataParameterCollection.Stub(m => m[Arg<string>.Is.Anything])
                .WhenCalled(e => e.ReturnValue = _dataParameterCollection.SingleOrDefault(m => string.CompareOrdinal((string) e.Arguments.ElementAt(0), m.ParameterName) == 0))
                .Return(null)
                .Repeat.Any();
                
            _expectedDbCommand = MockRepository.GenerateMock<IDbCommand>();
            _expectedDbCommand.Stub(m => m.CommandText)
                .Return(commandText)
                .Repeat.Any();
            _expectedDbCommand.Stub(m => m.CommandType)
                .Return(commandType)
                .Repeat.Any();
            _expectedDbCommand.Stub(m => m.CommandTimeout)
                .Return(commandTimeout)
                .Repeat.Any();
            _expectedDbCommand.Stub(m => m.Parameters)
                .WhenCalled(e => e.ReturnValue = dataParameterCollection)
                .Return(null)
                .Repeat.Any();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds a smallint data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddSmallIntDataParameter(string parameterName, int? value, int size, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.Int16, size, isNullable);
            return this;
        }

        /// <summary>
        /// Adds an int data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddIntDataParameter(string parameterName, int? value, int size, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.Int32, size, isNullable);
            return this;
        }

        /// <summary>
        /// Adds an bit data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddBitDataParameter(string parameterName, bool? value, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.UInt64, 1, isNullable);
            return this;
        }

        /// <summary>
        /// Adds a varchar data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddVarCharDataParameter(string parameterName, string value, int size, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.String, size, isNullable);
            return this;
        }

        /// <summary>
        /// Adds a char data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddCharDataParameter(string parameterName, string value, int size, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.StringFixedLength, size, isNullable);
            return this;
        }

        /// <summary>
        /// Adds a char data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddCharDataParameter(string parameterName, Guid value, bool isNullable = false)
        {
            AddCharDataParameter(parameterName, value.ToString("D").ToUpper(), 36, isNullable);
            return this;
        }

        /// <summary>
        /// Adds a text data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddTextDataParameter(string parameterName, string value, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, value == null ? DbType.AnsiString : DbType.String, isNullable: isNullable);
            return this;
        }

        /// <summary>
        /// Adds a date data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddDateParameter(string parameterName, DateTime? value, bool isNullable = false)
        {
            AddDataParameter(parameterName, value?.Date, DbType.Date, isNullable: isNullable);
            return this;
        }

        /// <summary>
        /// Adds a time data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddTimeParameter(string parameterName, DateTime? value, bool isNullable = false)
        {
            return AddTimeParameter(parameterName, value?.TimeOfDay, isNullable);
        }

        /// <summary>
        /// Adds a time data parameter.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        /// <returns>The builder which can build a database command tester.</returns>
        public IDbCommandTestBuilder AddTimeParameter(string parameterName, TimeSpan? value, bool isNullable = false)
        {
            AddDataParameter(parameterName, value, DbType.Time, isNullable: isNullable);
            return this;
        }

        /// <summary>
        /// Build the executor which can test a database command.
        /// </summary>
        /// <returns>Executor which can test a database command.</returns>
        public IDbCommandTestExecutor Build()
        {
            return new DbCommandTestExecutor(_expectedDbCommand);
        }

        /// <summary>
        /// Adds a data parameter to the data parameter collection.
        /// </summary>
        /// <param name="parameterName">The name of the data parameter.</param>
        /// <param name="value">The value for the data parameter.</param>
        /// <param name="dbType">The database type for the data parameter.</param>
        /// <param name="size">The size for the data parameter.</param>
        /// <param name="isNullable">Indicates whether the data parameter can be null.</param>
        private void AddDataParameter(string parameterName, object value, DbType dbType, int size = 0, bool isNullable = false)
        {
            ArgumentNullGuard.NotNullOrWhiteSpace(parameterName, nameof(parameterName));

            IDbDataParameter dataParameter = MockRepository.GenerateMock<IDbDataParameter>();
            dataParameter.Stub(m => m.ParameterName)
                .Return(parameterName)
                .Repeat.Any();
            dataParameter.Stub(m => m.Value)
                .Return(value ?? DBNull.Value)
                .Repeat.Any();
            dataParameter.Stub(m => m.DbType)
                .Return(dbType)
                .Repeat.Any();
            dataParameter.Stub(m => m.Size)
                .Return(size)
                .Repeat.Any();
            dataParameter.Stub(m => m.IsNullable)
                .Return(isNullable)
                .Repeat.Any();

            _dataParameterCollection.Add(dataParameter);
        }

        #endregion
    }
}
