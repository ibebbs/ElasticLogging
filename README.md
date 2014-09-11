ElasticLogging
==============

A custom event sync for SLAB allowing in or out of process logging of ETW events to ElasticSearch.

Simply copy the ElasticLogging assembly (and dependencies) to the SLAB out-of-process service directory and add it as a custom sync to the SemanticLogging-svc.xml file as shown below:

```xml
<?xml version="1.0" encoding="utf-8" ?>

<configuration
  xmlns="http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw"
  xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xsi:schemaLocation="http://schemas.microsoft.com/practices/2013/entlib/semanticlogging/etw SemanticLogging-svc.xsd">
               
  <!-- Optional settings for fine tuning performance and Trace Event Session identification-->
  <traceEventService/>
  
  <!-- Sinks reference definitons used by this host to listen ETW events -->
  <sinks>
    <customSink name="MyElasticLoggingSink" type ="ElasticLogging.ElasticSearchSink, ElasticLogging">
      <sources>
        <eventSource name="My-EventSource-Name" level="LogAlways" />
        <eventSource name="My-Other-EventSource-Name" level="LogAlways" />
      </sources>
      <parameters>
        <parameter name="connectionString" type="System.String" value="localhost:9200" />
        <parameter name="index" type="System.String" value="indexname" />
        <parameter name="appendDate" type="System.Boolean" value="true" />
        <parameter name="dateFormat" type="System.String" value="yyyy-MM-dd" />
      </parameters>
    </customSink>
  </sinks>
</configuration>
```
