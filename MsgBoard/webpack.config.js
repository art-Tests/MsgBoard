const path = require('path')
module.exports = {
  entry: {
    PostIndex: './src/Post/index.ts'
  },
  output: {
    filename: '[name].js',
    path: path.resolve(__dirname, 'dist')
  },
  resolve: {
    extensions: ['.webpack.js', '.web.js', '.ts', '.js']
  },

  module: {
    rules: [
      {
        test: /\.ts$/,
        loader: 'awesome-typescript-loader'
      }
    ]
  }
}
