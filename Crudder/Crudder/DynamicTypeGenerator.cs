using System.Reflection.Emit;
using System.Reflection;

public class DynamicTypeGenerator
{
    private static readonly Dictionary<string, Type> SqlTypeMapping = new()
    {
        { "int", typeof(int) },
        { "bigint", typeof(long) },
        { "smallint", typeof(short) },
        { "tinyint", typeof(byte) },
        { "bit", typeof(bool) },
        { "decimal", typeof(decimal) },
        { "numeric", typeof(decimal) },
        { "money", typeof(decimal) },
        { "float", typeof(double) },
        { "real", typeof(float) },
        { "datetime", typeof(DateTime) },
        { "smalldatetime", typeof(DateTime) },
        { "date", typeof(DateTime) },
        { "time", typeof(TimeSpan) },
        { "char", typeof(string) },
        { "varchar", typeof(string) },
        { "nchar", typeof(string) },
        { "nvarchar", typeof(string) },
        { "text", typeof(string) },
        { "ntext", typeof(string) },
        { "uniqueidentifier", typeof(Guid) },
        { "xml", typeof(string) },
        { "varbinary", typeof(byte[]) },
        { "binary", typeof(byte[]) }
    };

    public Type CreateDynamicType(string typeName, List<(string ColumnName, string DataType)> columns)
    {
        AssemblyName assemblyName = new(typeName + "Assembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(
            assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(typeName + "Module");
        TypeBuilder typeBuilder = moduleBuilder.DefineType(
            typeName, TypeAttributes.Public | TypeAttributes.Class);

        foreach (var column in columns)
        {
            string columnName = column.ColumnName;
            string sqlType = column.DataType;

            if (!SqlTypeMapping.TryGetValue(sqlType.ToLower(), out Type propertyType))
            {
                propertyType = typeof(object);
            }

            CreateProperty(typeBuilder, columnName, propertyType);
        }

        return typeBuilder.CreateType();
    }

    private void CreateProperty(TypeBuilder typeBuilder, string propertyName, Type propertyType)
    {
        FieldBuilder fieldBuilder = typeBuilder.DefineField(
            $"_{propertyName.ToLower()}", propertyType, FieldAttributes.Private);
        PropertyBuilder propertyBuilder = typeBuilder.DefineProperty(
            propertyName, PropertyAttributes.HasDefault, propertyType, null);

        MethodBuilder getMethodBuilder = typeBuilder.DefineMethod(
            $"get_{propertyName}", MethodAttributes.Public, propertyType, Type.EmptyTypes);
        ILGenerator getIlGenerator = getMethodBuilder.GetILGenerator();
        getIlGenerator.Emit(OpCodes.Ldarg_0);
        getIlGenerator.Emit(OpCodes.Ldfld, fieldBuilder);
        getIlGenerator.Emit(OpCodes.Ret);

        MethodBuilder setMethodBuilder = typeBuilder.DefineMethod(
            $"set_{propertyName}", MethodAttributes.Public, null, new[] { propertyType });
        ILGenerator setIlGenerator = setMethodBuilder.GetILGenerator();
        setIlGenerator.Emit(OpCodes.Ldarg_0);
        setIlGenerator.Emit(OpCodes.Ldarg_1);
        setIlGenerator.Emit(OpCodes.Stfld, fieldBuilder);
        setIlGenerator.Emit(OpCodes.Ret);

        propertyBuilder.SetGetMethod(getMethodBuilder);
        propertyBuilder.SetSetMethod(setMethodBuilder);
    }
}