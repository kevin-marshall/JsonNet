require 'dev'

task :pull do 
  Dir.glob('dep/**/*.dll') {|f|
    puts f
  }
end