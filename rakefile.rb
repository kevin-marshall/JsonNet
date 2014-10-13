require 'dev_tasks'

task :pull do 
  Dir.glob('dep/**/*.dll') {|f|
    puts f
  }
end

task :default => [:dev_tasks_default] do
end