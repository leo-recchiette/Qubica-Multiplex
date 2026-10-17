pipeline {
    agent any
    stages {
        stage('Setup') { steps { sh 'make setup' } }
        stage('Build') { steps { sh 'make build' } }
        stage('Test')  { steps { sh 'make test' } }
    }
    post {
        always { sh 'make teardown' }
    }
}
