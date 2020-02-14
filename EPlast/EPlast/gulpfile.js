/// <binding AfterBuild='default' Clean='clean' />
/*
This file is the main entry point for defining Gulp tasks and using Gulp plugins.
Click here to learn more. http://go.microsoft.com/fwlink/?LinkId=518007
*/

var gulp = require('gulp');
var del = require('del');

var paths = {
    scripts: ['wwwroot/scripts/**/*.js'],
};

gulp.task('clean', function () {
    return del(['wwwroot/js/tsCompiled/*']);
});

gulp.task('scripts', function () {
    gulp.src(paths.scripts).pipe(gulp.dest('wwwroot/js/tsCompiled'));
});

gulp.task('default', gulp.series('clean', 'scripts'));
/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    less = require("gulp-less"); // adding less module

var paths = {
    webroot: "./wwwroot/"
};
// registrating task for transforming styles.less into css file 
gulp.task("less", function () {
    return gulp.src('./wwwroot/less/styles.less')
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + '/css' ))
});