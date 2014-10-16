require 'dev_tasks'

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