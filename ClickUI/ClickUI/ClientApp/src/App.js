import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './pages/Layout';
import { Home } from './pages/Home';
import * as signalR from "@aspnet/signalr";

export default class App extends Component {
    constructor(props) {
        super(props);
        this.state = {
            signalR: {
                starUpdates: new StarUpdatesSR()
            }
        };

        this.state.signalR.starUpdates.connect();
    }

    render() {
        return (
            <div id="App">
                <Layout>
                    <Route exact path='/'
                        component={() =>
                            <Home starUpdatesSR={this.state.signalR.starUpdates} />
                        }
                    />
                </Layout>
            </div>
        );
    }
}

class StarUpdatesSR {
    constructor() {
        this.connection = undefined;
        this.connected = false;

        this.onConnectionReadyHandlers = [];
    }

    async connect() {
        const connection = new signalR.HubConnectionBuilder()
            .withUrl("http://www.api.stargazing.io/signalr/starupdates")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        await connection.start({ withCredentials: true });

        this.connection = connection;
        this.connected = true;
        this.onConnectionReadyHandlers.forEach(fn => fn(this.connection));

        return this;
    }

    onConnectionReady(fn) {
        if (this.connected) {
            fn(this.connection);
        } else {
            this.onConnectionReadyHandlers.push(fn);
        }
    }
}