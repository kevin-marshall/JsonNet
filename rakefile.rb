require 'dev_tasks'

task :update_dependencies do
  Console.announce_task_start 'update_dependencies'
  if(DEV_TASKS.has_key?(:branch) && DEV_TASKS[:branch]=='develop')
    # QcNet.dll,nunit.framework.dll
    ["nunit.framework.dll","QcNet.dll"].each{|f|
      source = "#{Environment.dev_root}/wrk/github/lou-parslow/QcNet/bin/Net4.0/Release/#{f}"
	  dest = "./dep/#{f}"
      if(File.exists?(source))
	    if(File.exists?(dest))
	      FileUtils.cp(source,dest) if(File.mtime(source) > File.mtime(dest))
	    else
	      FileUtils.cp(source,dest)
	    end
	  end
    }
  end
end

task :default => [:update_dependencies,:dev_tasks_default] do
end

#
# Transformations
#   Hash/Array to XML (Element)
#   XML (Element) to Hash/Array
#
#  since XML is more compilicationed specification than JSON,
#  working on the XML to JSON transformation first would provide insight
#  into the potentially simpler operation of transforming JSON to XML