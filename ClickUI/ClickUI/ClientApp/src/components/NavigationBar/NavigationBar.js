import React, { Component } from "react";
import { withRouter } from "react-router-dom";

import styles from "./NavigationBar.module.css";
import { HamburgerMenu } from "./HamburgerMenu";

class NavigationBar extends Component {
    constructor(props) {
        super(props);

        this.goTitlePage = this.goTitlePage.bind(this);
        this.closeDropdown = this.closeDropdown.bind(this);
        this.toggleDropdown = this.toggleDropdown.bind(this);

        this.state = {
            dropdown: {
                isOpen: false
            }
        };
    }

    goTitlePage() {
        this.props.history.push(this.props.titlePage);
        this.closeDropdown();
    }

    async toggleDropdown() {
        const dropdown = {
            isOpen: !this.state.dropdown.isOpen
        };

        await this.setState({ dropdown });
    }

    async closeDropdown() {
        const dropdown = {
            isOpen: false
        };

        await this.setState({ dropdown });
    }

    render() {
        return (
            <div className={styles.Container}>
                <div className={styles.NavigationBar}>
                    {/* NavigationBar__Balance is an element to allow flex to center the NavigationBar__Title element. */}
                    <div className={styles.NavigationBar__Balance} />
                    <div className={styles.NavigationBar__Title} onClick={this.goTitlePage}>
                        {this.props.title}
                    </div>
                    <ul
                        className={
                            this.state.dropdown.isOpen
                                ? `${styles.NavigationBar__Options} ${styles.Open}`
                                : styles.NavigationBar__Options}
                    >
                        {this.props.children}
                    </ul>
                    <HamburgerMenu onToggle={this.toggleDropdown}/>
                </div>
            </div>
        );
    }
}

const NavigationBarHOC = withRouter(NavigationBar);

export { NavigationBarHOC as NavigationBar };