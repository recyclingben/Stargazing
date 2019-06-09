import argparse
import os
from enum import Enum

class Service():
    def __init__(self, title, location):
        self.title = title
        self.location = location

    def rebuild(self): 
        os.system("docker build -t {}:1.0.0-a0 -f {}/{}/Dockerfile .".format(self.title, self.location, self.location))
        os.system("kubectl delete deployment {}".format(self.title))
        os.system("kubectl delete service {}".format(self.title))
        os.system("kubectl create -f {}/{}-deployment.yml".format(self.location, self.title))

CLICKUI = Service("clickui", "ClickUI")
APIGATEWAY = Service("apigateway", "APIGateway")
STARS = Service("stars", "Stars")

class Ingress():
    def __init__(self, title, location):
        self.title = title
        self.location = location

    def rebuild(self): 
        os.system("kubectl delete ingress.extensions {}".format(self.title))
        os.system("kubectl create -f {}/{}-deployment.yml".format(self.location, self.title))
    
INGRESS = Ingress("ingress", "Ingress")

buildables = [
    INGRESS, CLICKUI, STARS, APIGATEWAY
]

def argument_string_to_bool(v):
    if v.lower() in ("yes", "true", "t", "y", "1"):
        return True
    elif v.lower() in ("no", "false", "f", "n", "0"):
        return False
    else:
        raise argparse.ArgumentTypeError("Expected value parseable to boolean, but got `{}`.".format(v))


def main():
    parser = argparse.ArgumentParser(description='Automatically configure kubectl deployments.')
    group = parser.add_mutually_exclusive_group()
    all_arg = group.add_argument("-a", "--all", dest="all",
                        type=argument_string_to_bool,
                        default=False)

    name_arg = group.add_argument("-n", "--name", type=str)

    args = parser.parse_args()

    if (args.all):
        for buildable in buildables:
            buildable.rebuild()
    else:
        if args.name is None:
            raise argparse.ArgumentError(name, "Flag --all was set to false, but no name was specified.")
        buildable = next(f for f in buildables if f.title == args.name)
        buildable.rebuild()

if __name__ == "__main__":
    main()