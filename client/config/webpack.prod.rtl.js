const webpack = require('webpack');
const path = require('path');
const MiniCssExtractPlugin = require('mini-css-extract-plugin');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TerserJSPlugin = require('terser-webpack-plugin');
const OptimizeCSSAssetsPlugin = require('optimize-css-assets-webpack-plugin');

module.exports = {
  entry: {
    'app-rtl': './src/index-rtl.js',
  },
  output: {
    path: path.join(__dirname, '../build/fa'),
    filename: '[name].[chunkhash:8].bundle.js',
    chunkFilename: '[name].[chunkhash:8].chunk.js',
  },
  mode: 'production',
  module: {
    rules: [
      {
        test: /\.m?js$/,
        exclude: /(node_modules|bower_components)/,
        use: {
          loader: 'babel-loader', // transpiling our JavaScript files using Babel and webpack
        },
      },
      {
        test: /\.(sa|sc|c)ss$/,
        use: [
          MiniCssExtractPlugin.loader,
          'css-loader', // translates CSS into CommonJS
          'postcss-loader', // Loader for webpack to process CSS with PostCSS
          'sass-loader', // compiles Sass to CSS, using Node Sass by default
        ],
      },
      {
        test: /\.(png|svg|jpe?g|gif)$/i,
        use: [
          {
            loader: 'file-loader', // This will resolves import/require() on a file into a url and emits the file into the output directory.
            options: {
              name: '[name].[ext]',
              outputPath: '../assets/img',
              esModule: false,
            },
          },
        ],
      },
      {
        test: /\.(woff|woff2|eot|ttf|otf)$/,
        use: [
          {
            loader: 'file-loader', // This will resolves import/require() on a file into a url and emits the file into the output directory.
            options: {
              name: '[name].[ext]',
              outputPath: '../assets/font',
            },
          },
        ],
      },
      {
        test: /\.html$/i,
        use: {
          loader: 'html-loader',
          options: {
            attributes: {
              list: [
                {
                  tag: 'img',
                  attribute: 'src',
                  type: 'src',
                },
                {
                  tag: 'img',
                  attribute: 'srcset',
                  type: 'srcset',
                },
                {
                  tag: 'img',
                  attribute: 'data-src',
                  type: 'src',
                },
                {
                  tag: 'img',
                  attribute: 'data-srcset',
                  type: 'srcset',
                },
              ],
              //root: path.join(__dirname, '../build'),
            },
          },
        },
      },
    ],
  },
  optimization: {
    minimizer: [new TerserJSPlugin(), new OptimizeCSSAssetsPlugin()],
    splitChunks: {
      cacheGroups: {
        commons: {
          test: /[\\/]node_modules[\\/]/,
          name: 'vendors',
          chunks: 'all',
        },
      },
      chunks: 'all',
    },
    runtimeChunk: {
      name: 'runtime',
    },
  },
  plugins: [
    // load jQuery
    new webpack.ProvidePlugin({
      $: 'jquery',
      jQuery: 'jquery',
    }),
    // CleanWebpackPlugin will do some clean up/remove folder before build
    // In this case, this plugin will remove 'dist' and 'build' folder before re-build again
    new CleanWebpackPlugin(),
    // This plugin will extract all css to one file
    new MiniCssExtractPlugin({
      filename: '[name].[chunkhash:8].bundle.css',
      chunkFilename: '[name].[chunkhash:8].chunk.css',
    }),
    // The plugin will generate an HTML5 file for you that includes all your webpack bundles in the body using script tags
    new HtmlWebpackPlugin({
      chunks: ['app-rtl'],
      template: 'public/fa/index.html',
      filename: 'fa/index.html',
    }),
  ],
};
