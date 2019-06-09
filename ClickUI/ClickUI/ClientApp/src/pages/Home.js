import React, { Component } from "react";
import { StarPanel }from "../components/StarIncrementor/StarPanel";

export class Home extends Component {
    constructor(props) {
        super(props);
        this.state = { };
    }

    render() {
        return (
            <div id="Wrapper">
                <StarPanel
                    starUpdatesSR={this.props.starUpdatesSR}
                />
            </div>
        );
    }
}
