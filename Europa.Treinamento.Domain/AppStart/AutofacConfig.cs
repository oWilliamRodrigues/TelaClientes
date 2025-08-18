using Autofac;
using System;
using System.Collections.Generic;
using System.Reflection;
using Europa.Domain.Shared.Commons;

namespace Europa.Treinamento.Domain.AppStart
{
    public static class AutofacConfig
    {
        public static ContainerBuilder Register(ContainerBuilder builder)
        {
            return Register(builder, instancePerRequest: true);
        }

        public static ContainerBuilder RegisterForDevelopment(ContainerBuilder builder)
        {
            return Register(builder, instancePerRequest: false);
        }
        
        public static ContainerBuilder RegisterWithoutInstancePerRequest(ContainerBuilder builder)
        {
            return Register(builder, instancePerRequest: false);
        }

        private static ContainerBuilder Register(ContainerBuilder builder, bool instancePerRequest)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            if (instancePerRequest)
            {
                foreach (var injectionList in InjectedClassesList(currentAssembly))
                {
                    RegistrarDependenciasInstanciandoPorRequest(builder, injectionList);
                }
            }
            else
            {
                foreach (var injectionList in InjectedClassesList(currentAssembly))
                {
                    RegistrarDependencias(builder, injectionList);
                }
            }

            return builder;
        }

        private static void RegistrarDependencias(ContainerBuilder builder, List<Type> injectionList)
        {
            builder.RegisterTypes(injectionList.ToArray());
        }

        private static void RegistrarDependenciasInstanciandoPorRequest(ContainerBuilder builder,
                                                                        List<Type>       injectionList)
        {
            builder.RegisterTypes(injectionList.ToArray()).PropertiesAutowired().InstancePerRequest();
        }

        private static List<List<Type>> InjectedClassesList(Assembly currentAssembly)
        {
            return new List<List<Type>>
                   {
                       ReflectionHelper.ClassInNamespace(Layers.Repository,     currentAssembly),
                       ReflectionHelper.ClassInNamespace(Layers.Services,       currentAssembly)
                   };
        }
    }
}