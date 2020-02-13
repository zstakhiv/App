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