/// <binding AfterBuild='default' Clean='clean' />

var gulp = require('gulp');
var del = require('del');
var less = require('gulp-less');
var sass = require('gulp-sass');

var modules = ['bootstrap', 'jquery', 'jquery-ui-dist', 'mdbootstrap', 'popper.js'];
var paths = {
    scripts: ['wwwroot/uncompiled/ts/**/*.js'],
    webroot: 'wwwroot/'
};

gulp.task('clean', function () {
    return del(['wwwroot/compiled/js/*', 'wwwroot/compiled/css/*']);
});

gulp.task('scripts', function () {
    return gulp.src(paths.scripts)
        .pipe(gulp.dest('wwwroot/compiled/js'));
});

gulp.task('less', function () {
    return gulp.src('wwwroot/uncompiled/less/**/*.less')
        .pipe(less())
        .pipe(gulp.dest(paths.webroot + 'compiled/css'));
});

gulp.task("sass", function () {
    return gulp.src('wwwroot/uncompiled/sass/**/*.scss')
        .pipe(sass())
        .pipe(gulp.dest(paths.webroot + 'compiled/css'));
});

async function ClearLib() {
    return del('wwwroot/lib/*');
};
async function ModulesToLib() {
    gulp
    try {
        for (var i = 0; i < modules.length; i++) {
            gulp.src('node_modules/' + modules[i] + '/**')
                .pipe(gulp.dest(paths.webroot + 'lib/' + modules[i]));
        }
    }
    catch (e) {
        return -1;
    }
    return 0;
};

gulp.task('style', gulp.parallel('less', 'sass'));

gulp.task('default', gulp.series('clean', 'scripts', 'style'));

gulp.task("update-lib", gulp.series(ClearLib, ModulesToLib));