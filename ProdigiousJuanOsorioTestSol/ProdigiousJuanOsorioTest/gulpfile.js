//plug-ins
var gulp = require('gulp'); 

var concat = require('gulp-concat');
var uglify = require('gulp-uglify');
var autoprefix = require('gulp-autoprefixer');
var minifyCSS = require('gulp-minify-css');

//concat and minify
gulp.task('javaScripts', function() {
  gulp.src(['./Js/prodigious.js'])
    .pipe(concat('prodigious.min.js'))    
    .pipe(uglify())
    .pipe(gulp.dest('./Js/prod/'));
});

// CSS concat, auto-prefix and minify
gulp.task('styles', function() {
  gulp.src(['./Css/*.css'])
    .pipe(concat('prodigious.min.css'))
    .pipe(autoprefix('last 2 versions'))
    .pipe(minifyCSS())
    .pipe(gulp.dest('./Css/prod/'));
});

gulp.task('default', ['javaScripts', 'styles'], function() {
});