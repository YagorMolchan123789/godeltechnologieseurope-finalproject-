const webpack = require('webpack');
const BundleAnalyzerPlugin =
    require('webpack-bundle-analyzer').BundleAnalyzerPlugin;
const Dotenv = require('dotenv-webpack');

module.exports = {
    mode: 'development',
    devtool: 'cheap-module-source-map',
    devServer: {
        hot: true,
        open: true,
    },
    module: {
        rules: [
            {
                test: /\.(ts|js)x?$/,
                exclude: /node_modules/,
                use: [
                    {
                        loader: 'babel-loader',
                        options: {
                            presets: [
                                [
                                    '@babel/preset-env',
                                    {
                                        targets: 'defaults',
                                    },
                                ],
                            ],
                        },
                    },
                ],
            },
        ],
    },
    plugins: [
        new Dotenv(),
        new webpack.DefinePlugin({
            'process.env.name': JSON.stringify('Alchemic'),
        }),
        new BundleAnalyzerPlugin(),
    ],
};
