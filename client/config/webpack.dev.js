const path = require('path');
const HtmlWebpackPlugin = require('html-webpack-plugin');
const { CleanWebpackPlugin } = require('clean-webpack-plugin');

module.exports = {
  entry: {
    app: './src/index.js',
  },
  output: {
    path: path.join(__dirname, '../build'),
    filename: '[name].bundle.js',
  },
  mode: 'development',
  devServer: {
    contentBase: path.join(__dirname, '../build'),
    compress: true,
    hot: true,
    inline: true,
    port: 3000,
    overlay: true,
  },
  devtool: 'cheap-module-eval-source-map',
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
        test: /\.(le|c)ss$/,
        use: [
          'style-loader', // creates style nodes from JS strings
          'css-loader', // translates CSS into CommonJS
          'postcss-loader', // Loader for webpack to process CSS with PostCSS
          {
            loader: 'less-loader',
            options: {
              lessOptions: {
                javascriptEnabled: true,
              },
            },
          }, // compiles less to CSS, using Node less by default
          //'sass-loader', // compiles Sass to CSS, using Node Sass by default
        ],
      },
      {
        test: /\.(png|svg|jpe?g|gif|ico)$/i,
        use: [
          {
            loader: 'file-loader', // This will resolves import/require() on a file into a url and emits the file into the output directory.

            options: {
              name: '[name].[ext]',
              outputPath: 'assets/img',
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
              outputPath: 'assets/font',
            },
          },
        ],
      },
      {
        test: /\.html$/,
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
  plugins: [
    // CleanWebpackPlugin will do some clean up/remove folder before build
    // In this case, this plugin will remove 'dist' and 'build' folder before re-build again
    new CleanWebpackPlugin(),

    // The plugin will generate an HTML5 file for you that includes all your webpack bundles in the body using script tags
    new HtmlWebpackPlugin({
      //inject: false,
      chunks: ['app'],
      template: 'public/index.html',
      filename: 'index.html',
    }),
  ],
};
