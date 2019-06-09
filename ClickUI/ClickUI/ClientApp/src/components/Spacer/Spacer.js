import React, { Component } from "react";

export class Spacer extends Component {
    render() {
        return (
            <div id="Spacer">
                <div style={{ height: this.props.height }} />
            </div>
        );
    }
}

export const SPACER_HEIGHT = {
    small: 10,
    medium: 15,
    large: 30
};