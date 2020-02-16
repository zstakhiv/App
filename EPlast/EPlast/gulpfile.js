/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');
var less = require('gulp-less'); // adding less module
var sass = require('gulp-sass'); // adding sass module

var paths = {
    scripts: ['wwwroot/scripts/**/*.js'],
    webroot: 'wwwroot/'
};

gulp.task('clean', function () {
    return del(['wwwroot/js/tsCompiled/*', 'wwwroot/css/lessCompiled/*']);
});

gulp.task('scripts', function () {
    return gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/js/tsCompiled'));
});

// registrating task for transforming styles.less into css file 
gulp.task('less', function () {
    return gulp.src('wwwroot/less/**/*.less')
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + 'css/lessCompiled'));
});

gulp.task("sass", function () {
    return gulp.src('wwwroot/sass/**/*.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'css/sassCompiled'));
});

gulp.task('build', gulp.parallel('scripts', 'less'));

gulp.task('build', gulp.parallel('scripts', 'sass'));

gulp.task('default', gulp.series('clean', 'build'));