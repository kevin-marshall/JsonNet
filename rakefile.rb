require 'dev_tasks'

DEV_TASKS[:dependencies][:rake]=["#{Environment.dev_root}/wrk/github/lou-parslow/QcNet"]

DEV_TASKS[:dependencies][:source]=
["#{Environment.dev_root}/dep/github/lou-parslow/NetLibs/develop/NUnit/2.6.3/bin/framework/nunit.framework.dll",
 "#{Environment.dev_root}/dep/github/lou-parslow/QcNet/develop/bin/Net4.0/Release/QcNet.dll"]
 
DEV_TASKS.update 

task :default => [:dev_tasks_default] do
end

#
# Transformations
#   Hash/Array to XML (Element)
#   XML (Element) to Hash/Array
#
#  since XML is more compilicationed specification than JSON,
#  working on the XML to JSON transformation first would provide insight
#  into the potentially simpler operation of transforming JSON to XML