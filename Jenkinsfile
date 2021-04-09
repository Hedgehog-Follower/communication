pipeline {
  agent any
  stages {
    stage('stage1') {
      steps {
        echo 'This is the build number: ${BUILD_NUMBER} of demo ${DEMO}'
      }
    }

  }
  environment {
    DEMO = '1'
  }
}