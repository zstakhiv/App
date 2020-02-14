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