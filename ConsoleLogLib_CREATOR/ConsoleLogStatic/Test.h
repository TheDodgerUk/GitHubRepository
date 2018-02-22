#pragma once
#include <iostream>
class interface {
public:
	static interface* CreateInstance();
	virtual void draw() = 0;
	virtual void set(int) = 0;
};

class implementation : public interface {
	int private_int_;
	void ReportValue_();
public:
	implementation();
	void draw();
	void set(int new_int);
};

implementation::implementation() {
	// your actual constructor goes here
}

void implementation::draw() {
	std::cout << "Implementation class draws something" << std::endl;
	ReportValue_();
}

void implementation::ReportValue_() {
	std::cout << "Private value is: " << private_int_ << std::endl;
}
void implementation::set(int new_int) {
	private_int_ = new_int;
}
interface* interface::CreateInstance() {
	return new implementation;
}