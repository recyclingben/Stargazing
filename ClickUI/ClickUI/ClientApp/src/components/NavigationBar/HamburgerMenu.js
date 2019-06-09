import React, { Component } from "react";

import styles from "./HamburgerMenu.module.css";

class HamburgerMenu extends Component {
    render() {
        return (
            <div className={styles.Container}>
                <button className={styles.HamburgerMenu} onClick={this.props.onToggle}>
                    <i className={"fas fa-bars"} />
                </button>
            </div>
        );
    }
}

export { HamburgerMenu };