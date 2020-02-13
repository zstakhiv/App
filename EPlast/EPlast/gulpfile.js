/// <binding Clean='clean' />
"use strict";

var gulp = require("gulp"),
    sass = require("gulp-sass"); // добавляем модуль sass

var paths = {
    webroot: "./wwwroot/"
};

gulp.task("sass", function () {
    return gulp.src('Sass/styles2.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + '/css'));
});