import React, { Component } from "react";
import Media from "react-media";

export class ScreenSize extends Component {
    render() {
        let q1 = "all";
        let q2 = "all";

        if (this.props.min) q1 = `only screen and (min-width: ${this.props.min}px)`;
        if (this.props.max) q2 = `not screen and (min-width: ${this.props.max}px)`;

        return (
            <Media
                query={q1}
                render={() => <Media query={q2} render={() => this.props.children} />}
            />
        );
    }
}