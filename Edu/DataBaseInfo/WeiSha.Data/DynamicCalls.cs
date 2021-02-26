using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
namespace DataBaseInfo
{
	public static class DynamicCalls
	{
		private static IDictionary<MethodInfo, FastInvokeHandler> dictInvoker = new Dictionary<MethodInfo, FastInvokeHandler>();
		private static IDictionary<Type, FastCreateInstanceHandler> dictCreator = new Dictionary<Type, FastCreateInstanceHandler>();
		private static IDictionary<PropertyInfo, FastPropertyGetHandler> dictGetter = new Dictionary<PropertyInfo, FastPropertyGetHandler>();
		private static IDictionary<PropertyInfo, FastPropertySetHandler> dictSetter = new Dictionary<PropertyInfo, FastPropertySetHandler>();
		public static FastInvokeHandler GetMethodInvoker(MethodInfo methodInfo)
		{
			if (DynamicCalls.dictInvoker.ContainsKey(methodInfo))
			{
				return DynamicCalls.dictInvoker[methodInfo];
			}
			FastInvokeHandler result;
			lock (DynamicCalls.dictInvoker)
			{
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[]
				{
					typeof(object),
					typeof(object[])
				}, methodInfo.DeclaringType.Module);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				ParameterInfo[] parameters = methodInfo.GetParameters();
				Type[] array = new Type[parameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					if (parameters[i].ParameterType.IsByRef)
					{
						array[i] = parameters[i].ParameterType.GetElementType();
					}
					else
					{
						array[i] = parameters[i].ParameterType;
					}
				}
				LocalBuilder[] array2 = new LocalBuilder[array.Length];
				for (int j = 0; j < array.Length; j++)
				{
					array2[j] = iLGenerator.DeclareLocal(array[j], true);
				}
				for (int k = 0; k < array.Length; k++)
				{
					iLGenerator.Emit(OpCodes.Ldarg_1);
					DynamicCalls.EmitFastInt(iLGenerator, k);
					iLGenerator.Emit(OpCodes.Ldelem_Ref);
					DynamicCalls.EmitCastToReference(iLGenerator, array[k]);
					iLGenerator.Emit(OpCodes.Stloc, array2[k]);
				}
				if (!methodInfo.IsStatic)
				{
					iLGenerator.Emit(OpCodes.Ldarg_0);
				}
				for (int l = 0; l < array.Length; l++)
				{
					if (parameters[l].ParameterType.IsByRef)
					{
						iLGenerator.Emit(OpCodes.Ldloca_S, array2[l]);
					}
					else
					{
						iLGenerator.Emit(OpCodes.Ldloc, array2[l]);
					}
				}
				if (!methodInfo.IsStatic)
				{
					iLGenerator.EmitCall(OpCodes.Callvirt, methodInfo, null);
				}
				else
				{
					iLGenerator.EmitCall(OpCodes.Call, methodInfo, null);
				}
				if (methodInfo.ReturnType == typeof(void))
				{
					iLGenerator.Emit(OpCodes.Ldnull);
				}
				else
				{
					DynamicCalls.EmitBoxIfNeeded(iLGenerator, methodInfo.ReturnType);
				}
				for (int m = 0; m < array.Length; m++)
				{
					if (parameters[m].ParameterType.IsByRef)
					{
						iLGenerator.Emit(OpCodes.Ldarg_1);
						DynamicCalls.EmitFastInt(iLGenerator, m);
						iLGenerator.Emit(OpCodes.Ldloc, array2[m]);
						if (array2[m].LocalType.IsValueType)
						{
							iLGenerator.Emit(OpCodes.Box, array2[m].LocalType);
						}
						iLGenerator.Emit(OpCodes.Stelem_Ref);
					}
				}
				iLGenerator.Emit(OpCodes.Ret);
				FastInvokeHandler fastInvokeHandler = (FastInvokeHandler)dynamicMethod.CreateDelegate(typeof(FastInvokeHandler));
				DynamicCalls.dictInvoker[methodInfo] = fastInvokeHandler;
				result = fastInvokeHandler;
			}
			return result;
		}
		public static FastCreateInstanceHandler GetInstanceCreator(Type type)
		{
			if (DynamicCalls.dictCreator.ContainsKey(type))
			{
				return DynamicCalls.dictCreator[type];
			}
			FastCreateInstanceHandler result;
			lock (DynamicCalls.dictCreator)
			{
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, type, new Type[0], typeof(DynamicCalls).Module);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Newobj, type.GetConstructor(Type.EmptyTypes));
				iLGenerator.Emit(OpCodes.Ret);
				FastCreateInstanceHandler fastCreateInstanceHandler = (FastCreateInstanceHandler)dynamicMethod.CreateDelegate(typeof(FastCreateInstanceHandler));
				DynamicCalls.dictCreator[type] = fastCreateInstanceHandler;
				result = fastCreateInstanceHandler;
			}
			return result;
		}
		public static FastPropertyGetHandler GetPropertyGetter(PropertyInfo propInfo)
		{
			if (DynamicCalls.dictGetter.ContainsKey(propInfo))
			{
				return DynamicCalls.dictGetter[propInfo];
			}
			FastPropertyGetHandler result;
			lock (DynamicCalls.dictGetter)
			{
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, typeof(object), new Type[]
				{
					typeof(object)
				}, propInfo.DeclaringType.Module);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetGetMethod(), null);
				DynamicCalls.EmitBoxIfNeeded(iLGenerator, propInfo.PropertyType);
				iLGenerator.Emit(OpCodes.Ret);
				FastPropertyGetHandler fastPropertyGetHandler = (FastPropertyGetHandler)dynamicMethod.CreateDelegate(typeof(FastPropertyGetHandler));
				DynamicCalls.dictGetter[propInfo] = fastPropertyGetHandler;
				result = fastPropertyGetHandler;
			}
			return result;
		}
		public static FastPropertySetHandler GetPropertySetter(PropertyInfo propInfo)
		{
			if (DynamicCalls.dictSetter.ContainsKey(propInfo))
			{
				return DynamicCalls.dictSetter[propInfo];
			}
			FastPropertySetHandler result;
			lock (DynamicCalls.dictSetter)
			{
				DynamicMethod dynamicMethod = new DynamicMethod(string.Empty, null, new Type[]
				{
					typeof(object),
					typeof(object)
				}, propInfo.DeclaringType.Module);
				ILGenerator iLGenerator = dynamicMethod.GetILGenerator();
				iLGenerator.Emit(OpCodes.Ldarg_0);
				iLGenerator.Emit(OpCodes.Ldarg_1);
				DynamicCalls.EmitCastToReference(iLGenerator, propInfo.PropertyType);
				iLGenerator.EmitCall(OpCodes.Callvirt, propInfo.GetSetMethod(), null);
				iLGenerator.Emit(OpCodes.Ret);
				FastPropertySetHandler fastPropertySetHandler = (FastPropertySetHandler)dynamicMethod.CreateDelegate(typeof(FastPropertySetHandler));
				DynamicCalls.dictSetter[propInfo] = fastPropertySetHandler;
				result = fastPropertySetHandler;
			}
			return result;
		}
		private static void EmitCastToReference(ILGenerator ilGenerator, Type type)
		{
			if (type.IsValueType)
			{
				ilGenerator.Emit(OpCodes.Unbox_Any, type);
				return;
			}
			ilGenerator.Emit(OpCodes.Castclass, type);
		}
		private static void EmitBoxIfNeeded(ILGenerator ilGenerator, Type type)
		{
			if (type.IsValueType)
			{
				ilGenerator.Emit(OpCodes.Box, type);
			}
		}
		private static void EmitFastInt(ILGenerator ilGenerator, int value)
		{
			switch (value)
			{
			case -1:
				ilGenerator.Emit(OpCodes.Ldc_I4_M1);
				return;
			case 0:
				ilGenerator.Emit(OpCodes.Ldc_I4_0);
				return;
			case 1:
				ilGenerator.Emit(OpCodes.Ldc_I4_1);
				return;
			case 2:
				ilGenerator.Emit(OpCodes.Ldc_I4_2);
				return;
			case 3:
				ilGenerator.Emit(OpCodes.Ldc_I4_3);
				return;
			case 4:
				ilGenerator.Emit(OpCodes.Ldc_I4_4);
				return;
			case 5:
				ilGenerator.Emit(OpCodes.Ldc_I4_5);
				return;
			case 6:
				ilGenerator.Emit(OpCodes.Ldc_I4_6);
				return;
			case 7:
				ilGenerator.Emit(OpCodes.Ldc_I4_7);
				return;
			case 8:
				ilGenerator.Emit(OpCodes.Ldc_I4_8);
				return;
			default:
				if (value > -129 && value < 128)
				{
					ilGenerator.Emit(OpCodes.Ldc_I4_S, (sbyte)value);
					return;
				}
				ilGenerator.Emit(OpCodes.Ldc_I4, value);
				return;
			}
		}
	}
}
