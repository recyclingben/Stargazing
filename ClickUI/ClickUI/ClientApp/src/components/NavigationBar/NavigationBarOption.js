import React, { Component } from "react";
import { withRouter } from "react-router-dom";

import "./NavigationBarOption.css";

class NavigationBarOption extends Component {
    constructor(props) {
        super(props);

        this.goPage = this.goPage.bind(this);
    }

    goPage() {
        this.props.history.push(this.props.page);
    }

    render() {
        return (
            <div id={"NavigationBarOptionContainer"}>
                <li className={"NavigationBarOption"} onClick={() => {
                    this.goPage();
                    this.props.onClick();
                }}>
                    {this.props.children}
                </li>
            </div>
        );
    }
}

const NavigationBarOptionHOC = withRouter(NavigationBarOption);

export { NavigationBarOptionHOC as NavigationBarOption };