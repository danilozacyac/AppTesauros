﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.1433
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------



[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
[System.ServiceModel.ServiceContractAttribute(ConfigurationName="IManejaLista")]
public interface IManejaLista
{
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManejaLista/GetData", ReplyAction="http://tempuri.org/IManejaLista/GetDataResponse")]
    string[] GetData(int initialValue, int finalValue, string listId);
    
    [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IManejaLista/GenerateList", ReplyAction="http://tempuri.org/IManejaLista/GenerateListResponse")]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(string[]))]
    [System.ServiceModel.ServiceKnownTypeAttribute(typeof(object[]))]
    int GenerateList(string objeto, string metodo, object[] parametros, string fieldName);
}

[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public interface IManejaListaChannel : IManejaLista, System.ServiceModel.IClientChannel
{
}

[System.Diagnostics.DebuggerStepThroughAttribute()]
[System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "3.0.0.0")]
public partial class ManejaListaClient : System.ServiceModel.ClientBase<IManejaLista>, IManejaLista
{
    
    public ManejaListaClient()
    {
    }
    
    public ManejaListaClient(string endpointConfigurationName) : 
            base(endpointConfigurationName)
    {
    }
    
    public ManejaListaClient(string endpointConfigurationName, string remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ManejaListaClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(endpointConfigurationName, remoteAddress)
    {
    }
    
    public ManejaListaClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
            base(binding, remoteAddress)
    {
    }
    
    public string[] GetData(int initialValue, int finalValue, string listId)
    {
        return base.Channel.GetData(initialValue, finalValue, listId);
    }
    
    public int GenerateList(string objeto, string metodo, object[] parametros, string fieldName)
    {
        return base.Channel.GenerateList(objeto, metodo, parametros, fieldName);
    }
}